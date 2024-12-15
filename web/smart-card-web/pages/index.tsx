import { useEffect, useState } from "react";

import DefaultLayout from "@/layouts/default";
import { GetAllTopicOutput } from "@/api/service-proxy";
import apiClient from "@/api/api-instance";
import TopicCard from "@/components/topic-card";

export default function IndexPage() {
  const [topics, setTopics] = useState<GetAllTopicOutput[]>([]);

  useEffect(() => {
    apiClient.topic.topicList().then((res) => {
      setTopics(res.data);
    });
  }, []);

  return (
    <DefaultLayout>
      <section className="flex flex-wrap justify-center gap-4">
        {topics.map((topic: GetAllTopicOutput) => (
          <TopicCard key={topic.id} topic={topic} />
        ))}
      </section>
    </DefaultLayout>
  );
}
