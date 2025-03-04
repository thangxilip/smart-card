import { Card, CardBody, CardFooter, CardHeader } from "@nextui-org/card";
import { Chip } from "@nextui-org/chip";
import { Avatar } from "@nextui-org/avatar";
import { Divider } from "@nextui-org/divider";
import NextLink from "next/link";

import { GetTopicStatisticsOutput } from "@/api/service-proxy";
import useUserStore from "@/stores/use-user-store";

interface TopicCardProps {
  topic: GetTopicStatisticsOutput;
}

const TopicCard = (props: TopicCardProps) => {
  const { topic } = props;
  const { user } = useUserStore();

  return (
    <NextLink href={`/topic/${topic.id}`}>
      <Card className="w-64 h-44 text-center flex items-center justify-center rounded-lg shadow-lg border-1 border-gray-200">
        <CardHeader className="flex gap-3">
          <p className="text-md font-500">{topic.name}</p>
        </CardHeader>
        <CardBody>
          <div className="flex gap-2">
            <Chip className="text-xs" color={"primary"}>
              New ({topic.newCards})
            </Chip>
            <Chip className="text-xs" color={"success"}>
              Learn ({topic.learningCards})
            </Chip>
            <Chip className="text-xs" color={"warning"}>
              Due ({topic.dueCards})
            </Chip>
          </div>
        </CardBody>
        <Divider />
        <CardFooter className="flex items-center text-xs">
          <Avatar
            className="mr-2"
            size="sm"
            src={"https://i.pravatar.cc/150?u=a042581f4e29026024d"}
          />{" "}
          Created by {`${user?.firstName} ${user?.lastName}`}
        </CardFooter>
      </Card>
    </NextLink>
  );
};

export default TopicCard;
