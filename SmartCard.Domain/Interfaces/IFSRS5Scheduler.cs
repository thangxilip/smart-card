using SmartCard.Domain.Enums;
using SmartCard.Domain.Models.FSRS;

namespace SmartCard.Domain.Interfaces;

public interface IFSRS5Scheduler
{
    (FSRSCard, ReviewLog) Review(FSRSCard fsrsCard, CardRating rating, DateTime reviewTime, bool sameDayReview = false);
}