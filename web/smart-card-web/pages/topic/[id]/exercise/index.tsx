import { useEffect, useState } from "react";
import { useRouter } from "next/router";
import { Card, CardBody, CardFooter, Button } from "@nextui-org/react";
import { Spacer } from "@nextui-org/spacer";
import { HttpStatusCode } from "axios";

import DefaultLayout from "@/layouts/default";
import {
  CardRating,
  GetDueCardsOutput,
  ReviseCardCommand,
} from "@/api/service-proxy";
import apiClient from "@/api/api-instance";
import CongratulationsCard from "@/pages/topic/components/congratulations-card";

const Exercise = () => {
  const router = useRouter();
  const [topicId, setTopicId] = useState<string | null>(null);
  const [cards, setCards] = useState<GetDueCardsOutput[]>([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [showAnswer, setShowAnswer] = useState(false);

  useEffect(() => {
    if (router.isReady) {
      const topicId = router.query.id as string;

      if (topicId) {
        setTopicId(topicId);
        apiClient.card.getCard({ topicId }).then((res) => {
          setCards(res.data);
        });
      }
    }
  }, [router.isReady]);

  const onShowAnswer = () => {
    setShowAnswer(true);
  };

  const onNext = async (rating: CardRating) => {
    if (cards[currentIndex]) {
      const response = await apiClient.card.reviseCreate({
        cardId: cards[currentIndex].id,
        rating: rating,
      } as ReviseCardCommand);

      if (response.status === HttpStatusCode.Ok) {
        setShowAnswer(false);
        if (currentIndex < cards.length - 1) {
          setCurrentIndex((prev) => prev + 1);
        } else {
          setCurrentIndex(-1);
        }
      }
    }
  };

  return (
    <DefaultLayout>
      {currentIndex >= 0 && (
        <Card
          className={`flex w-2/3 h-[50vh] m-auto p-10 perspective ${showAnswer ? "rotate-y-180" : ""}`}
        >
          <CardBody className={`flex justify-center items-center`}>
            <h1
              className={`text-lg font-medium ${showAnswer ? "rotate-y-180" : ""}`}
            >
              {showAnswer
                ? cards[currentIndex]?.front
                : cards[currentIndex]?.back}
            </h1>
          </CardBody>
          {!showAnswer && (
            <CardFooter className="flex justify-center">
              <Button
                className="text-white"
                color={"success"}
                size={"sm"}
                onPress={onShowAnswer}
              >
                Show answer
              </Button>
            </CardFooter>
          )}
          {showAnswer && (
            <CardFooter
              className={`flex justify-center gap-3 ${showAnswer ? "rotate-y-180" : ""}`}
            >
              <Button size={"sm"} onPress={() => onNext(CardRating.Again)}>
                Again
              </Button>
              <Button size={"sm"} onPress={() => onNext(CardRating.Hard)}>
                Hard
              </Button>
              <Button size={"sm"} onPress={() => onNext(CardRating.Good)}>
                Good
              </Button>
              <Button size={"sm"} onPress={() => onNext(CardRating.Easy)}>
                Easy
              </Button>
            </CardFooter>
          )}
        </Card>
      )}
      {currentIndex === -1 && (
        <div className={"flex flex-col justify-center items-center"}>
          <CongratulationsCard /> <Spacer y={5} />
          <Button
            className={"w-fit"}
            onPress={() => router.push(`/topic/${topicId}`)}
          >
            Back to topic
          </Button>
        </div>
      )}
    </DefaultLayout>
  );
};

export default Exercise;
