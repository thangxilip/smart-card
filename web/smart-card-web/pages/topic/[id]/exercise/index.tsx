import { useEffect, useState } from "react";
import { useRouter } from "next/router";
import { Card, CardBody, CardFooter, Button } from "@nextui-org/react";
import { Spacer } from "@nextui-org/spacer";
import { HttpStatusCode } from "axios";

import DefaultLayout from "@/layouts/default";
import { GetCardsForStudyingOutput, Score } from "@/api/service-proxy";
import apiClient from "@/api/api-instance";
import CongratulationsCard from "@/pages/topic/components/congratulations-card";

const Exercise = () => {
  const router = useRouter();
  const [topicId, setTopicId] = useState<string | null>(null);
  const [cards, setCards] = useState<GetCardsForStudyingOutput[]>([]);
  const [index, setIndex] = useState(0);
  const [showAnswer, setShowAnswer] = useState(false);

  useEffect(() => {
    if (router.isReady) {
      const topicId = router.query.id as string;

      if (topicId) {
        setTopicId(topicId);
        apiClient.topic.exerciseDetail(topicId).then((res) => {
          setCards(res.data);
        });
      }
    }
  }, [router.isReady]);

  const onShowAnswer = () => {
    setShowAnswer(true);
  };

  const onNext = async (score: Score) => {
    if (cards[index]) {
      const response = await apiClient.topic.scorePartialUpdate(topicId!, [
        {
          id: cards[index].id!,
          score,
        },
      ]);

      if (response.status === HttpStatusCode.Ok) {
        setShowAnswer(false);
        if (index < cards.length - 1) {
          setIndex((prev) => prev + 1);
        } else {
          setIndex(-1);
        }
      }
    }
  };

  return (
    <DefaultLayout>
      {index >= 0 && (
        <Card
          className={`flex w-2/3 h-[50vh] m-auto p-10 perspective ${showAnswer ? "rotate-y-180" : ""}`}
        >
          <CardBody className={`flex justify-center items-center`}>
            <h1
              className={`text-lg font-medium ${showAnswer ? "rotate-y-180" : ""}`}
            >
              {showAnswer
                ? cards[index]?.definition
                : cards[index]?.terminology}
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
              <Button size={"sm"} onPress={() => onNext(Score.Forgotten)}>
                Forgotten
              </Button>
              <Button size={"sm"} onPress={() => onNext(Score.Poor)}>
                Poor
              </Button>
              <Button size={"sm"} onPress={() => onNext(Score.Moderate)}>
                Moderate
              </Button>
              <Button size={"sm"} onPress={() => onNext(Score.Good)}>
                Good
              </Button>
              <Button size={"sm"} onPress={() => onNext(Score.Perfect)}>
                Perfect
              </Button>
            </CardFooter>
          )}
        </Card>
      )}
      {index === -1 && (
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
