import { Button } from "@nextui-org/button";
import { useEffect, useState } from "react";
import { useDisclosure } from "@nextui-org/modal";
import { useRouter } from "next/navigation";

import { GetTopicStatisticsOutput } from "@/api/service-proxy";
import TopicCard from "@/components/topic-card";
import useUserStore from "@/stores/use-user-store";
import apiClient from "@/api/api-instance";
import LoginModal from "@/components/modals/login";

const Dashboard = () => {
  const router = useRouter();
  const { isOpen, onOpen, onOpenChange } = useDisclosure();
  const { user } = useUserStore();
  const [topics, setTopics] = useState<GetTopicStatisticsOutput[]>([]);

  useEffect(() => {
    if (user) {
      apiClient.topic.statisticsList().then((res) => {
        setTopics(res.data);
      });
    }
  }, [user]);

  return (
    <>
      {user && (
        <section className="flex flex-wrap justify-center gap-4">
          {topics.map((topic: GetTopicStatisticsOutput) => (
            <TopicCard key={topic.id} topic={topic} />
          ))}
        </section>
      )}
      {(!user || topics.length === 0) && (
        <div className="flex items-center justify-center">
          <div className="text-center max-w-xl mx-auto">
            <h1 className="text-3xl font-bold text-gray-900 mb-4">
              You have no cards yet
            </h1>

            <p className="text-gray-600 mb-6">
              Let create your own cards and master whatever you&apos;re learning
              with interactive flashcards.
            </p>

            <div className="space-y-4">
              {!user && (
                <Button
                  className="px-6 py-3 focus:ring focus:ring-blue-300"
                  color={"primary"}
                  onClick={onOpen}
                >
                  Sign up for free
                </Button>
              )}
              {user && (
                <Button
                  className="px-6 py-3 focus:ring focus:ring-blue-300"
                  color={"primary"}
                  onClick={() => router.push("/topic/create")}
                >
                  Create your first card
                </Button>
              )}
            </div>
          </div>
        </div>
      )}
      <LoginModal isOpen={isOpen} onOpenChange={onOpenChange} />
    </>
  );
};

export default Dashboard;
