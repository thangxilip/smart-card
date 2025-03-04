using SmartCard.Domain.Enums;
using SmartCard.Domain.Interfaces;
using SmartCard.Domain.Models.FSRS;

namespace SmartCard.Infrastructure.Services;

public class FSRS5Scheduler : IFSRS5Scheduler
{
    private readonly FSRSParameters _params;

    public FSRS5Scheduler(FSRSParameters? parameters = null)
    {
        _params = parameters ?? new FSRSParameters();
    }

    private double CalculateRetrievability(double stability, double elapsedDays)
    {
        if (stability <= 0) return 0;
        return Math.Exp(Math.Log(0.9) * elapsedDays / stability);
    }

    // Initial difficulty after first rating (D₀(G))
    private double InitialDifficulty(CardRating rating)
    {
        double w4 = _params.W[4]; // Initial difficulty when rating is Again
        double w5 = _params.W[5]; // Used in exponential calculation
        return w4 - Math.Exp(w5 * ((int)rating - 1)) + 1;
    }

    // Same-day stability (S'(S,G))
    private double SameDayStability(double stability, CardRating rating)
    {
        double w17 = _params.W[17];
        double w18 = _params.W[18];
        return stability * Math.Exp(w17 * ((int)rating - 3 + w18));
    }

    // Difficulty delta calculation (ΔD(G))
    private double CalculateDifficultyDelta(CardRating rating)
    {
        double w6 = _params.W[6];
        return -w6 * ((int)rating - 3);
    }

    // New difficulty calculation with linear damping (D')
    private double CalculateNewDifficulty(double difficulty, double deltaDifficulty)
    {
        return difficulty + deltaDifficulty * (10 - difficulty) / 9;
    }

    // Mean reversion for difficulty (D")
    private double MeanReversionDifficulty(double difficulty)
    {
        double w7 = _params.W[7];
        double d0_4 = InitialDifficulty(CardRating.Easy); // D₀(4) as target
        return w7 * d0_4 + (1 - w7) * difficulty;
    }

    private double UpdateStability(double difficulty, double stability, double retrievability, CardRating rating, bool isReview)
    {
        if (!isReview || stability == 0)
        {
            // Initial stability calculation
            double s = _params.W[0] * Math.Exp(_params.W[1] * ((int)rating - 1));
            return Math.Max(_params.MinimumStability, s);
        }

        if (rating == CardRating.Again)
        {
            // Stability calculation for Again rating
            return _params.W[8] * stability *
                   Math.Exp(_params.W[9] * (difficulty - 3)) *
                   Math.Pow(retrievability + 1, _params.W[10]);
        }

        // Normal stability update
        double hardPenalty = rating == CardRating.Hard ? _params.W[15] : 1.0;
        double easyBonus = rating == CardRating.Easy ? _params.W[16] : 1.0;

        double newStability = stability *
            (1 + easyBonus * hardPenalty *
            _params.W[11] * Math.Exp(_params.W[12] * (difficulty - 3)) *
            Math.Pow(stability, -_params.W[13]) *
            Math.Exp(_params.W[14] * (1 - retrievability)));

        return Math.Min(_params.MaximumStability, Math.Max(_params.MinimumStability, newStability));
    }

    public (FSRSCard, ReviewLog) Review(FSRSCard fsrsCard, CardRating rating, DateTime reviewTime, bool sameDayReview = false)
    {
        var isReview = fsrsCard.State == CardState.New;

        var reviewLog = new ReviewLog
        {
            ReviewTime = reviewTime,
            Rating = rating,
            State = fsrsCard.State,
            PreviousDifficulty = fsrsCard.Difficulty,
            PreviousStability = fsrsCard.Stability
        };

        // Calculate elapsed time
        if (fsrsCard.LastReview.HasValue)
        {
            reviewLog.ElapsedDays = (reviewTime - fsrsCard.LastReview.Value).TotalDays;
            reviewLog.ScheduledDays = fsrsCard.DueDate.HasValue ? 
                (fsrsCard.DueDate.Value - fsrsCard.LastReview.Value).TotalDays : 0;
        }

        // Calculate retrievability
        reviewLog.Retrievability = isReview ? 
            CalculateRetrievability(fsrsCard.Stability, reviewLog.ElapsedDays) : 0;

        // Handle first review
        if (!isReview)
        {
            fsrsCard.Difficulty = InitialDifficulty(rating);
            fsrsCard.Stability = UpdateStability(fsrsCard.Difficulty, 0, 0, rating, false);
            fsrsCard.State = rating == CardRating.Again ? CardState.Learning : CardState.Review;
        }
        else if (sameDayReview)
        {
            // Handle same-day review
            fsrsCard.Stability = SameDayStability(fsrsCard.Stability, rating);
        }
        else
        {
            // Normal review
            double deltaDifficulty = CalculateDifficultyDelta(rating);
            double newDifficulty = CalculateNewDifficulty(fsrsCard.Difficulty, deltaDifficulty);
            fsrsCard.Difficulty = MeanReversionDifficulty(newDifficulty);
            
            if (rating == CardRating.Again)
            {
                fsrsCard.State = CardState.Learning;
                fsrsCard.LapseCount++;
            }

            fsrsCard.Stability = UpdateStability(
                fsrsCard.Difficulty,
                fsrsCard.Stability,
                reviewLog.Retrievability,
                rating,
                true
            );
        }

        // Update card metadata
        fsrsCard.LastReview = reviewTime;
        fsrsCard.RepsCount++;

        // Calculate next interval
        double interval = fsrsCard.Stability * Math.Log(_params.RequestRetentionRate) / Math.Log(0.9);
        fsrsCard.DueDate = reviewTime.AddDays(interval);

        return (fsrsCard, reviewLog);
    }
}