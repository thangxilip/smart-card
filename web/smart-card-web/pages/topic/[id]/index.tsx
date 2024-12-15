import DefaultLayout from "@/layouts/default";
import { useEffect, useState } from "react";
import { GetAllTopicOutput, GetTopicByIdOutput } from "@/api/service-proxy";
import apiClient from "@/api/api-instance";
import { useRouter } from "next/router";
import { Divider } from "@nextui-org/divider";
import { Card, CardBody } from "@nextui-org/card";
import { FiEdit } from "react-icons/fi";

const TopicDetail = () => {
  const router = useRouter();

  const [topic, setTopic] = useState<GetTopicByIdOutput>(
    {} as GetAllTopicOutput,
  );

  useEffect(() => {
    if (router.isReady) {
      const topicId = router.query.id as string;

      apiClient.topic.topicDetail(topicId).then((res) => {
        setTopic(res.data);
      });
    }
  }, [router.isReady]);

  return (
    <DefaultLayout>
      <div className="md:flex md:items-center md:justify-between mb-4">
        <div className="min-w-0 flex-1">
          <h2 className="text-2xl/7 font-bold text-gray-900 sm:truncate sm:text-3xl sm:tracking-tight">
            {topic.name}
          </h2>
        </div>
        <div className="mt-4 flex md:ml-4 md:mt-0">
          <button
            className="inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
            type="button"
            onClick={() => router.push(`${topic.id}/edit`)}
          >
            Edit
          </button>
          <button
            className="ml-3 inline-flex items-center rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
            type="button"
          >
            Start studying
          </button>
        </div>
      </div>
      <Divider />

      <h1 className="my-6 font-bold text-base/7 text-gray-900">Terminologies</h1>
      <Card className="px-4">
        <CardBody>
          <ul className="divide-y divide-gray-100">
            {topic.cards?.map((card) => (
              <li key={card.id} className="flex justify-between gap-x-6 py-5">
                <div className="flex w-full">
                  <div className="w-1/2 min-w-0 gap-x-4">
                    <div className="min-w-0 flex-auto">
                      <p className="text-sm/6 font-semibold text-gray-900">{card.terminology}</p>
                    </div>
                  </div>
                  <div className="w-1/2 shrink-0 sm:flex sm:flex-col items-start">
                    <p className="text-sm/6 text-gray-900">{card.definition}</p>
                  </div>
                </div>
                <FiEdit />
              </li>
            ))}
          </ul>
        </CardBody>
      </Card>
    </DefaultLayout>
  );
};

export default TopicDetail;
