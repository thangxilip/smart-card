import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import { Button, Form, Input, Textarea } from "@nextui-org/react";
import { Card, CardBody, CardHeader } from "@nextui-org/card";
import { FiPlus, FiTrash } from "react-icons/fi";
import { nanoid } from "nanoid";
import { notFound } from "next/navigation";
import { toast } from "react-toastify";

import apiClient from "@/api/api-instance";
import { UpdateTopicInput } from "@/api/service-proxy";
import DefaultLayout from "@/layouts/default";

const TopicEdit = () => {
  const router = useRouter();
  const [topicId, setTopicId] = useState<string | null>(null);
  const [submitted, setSubmitted] = useState(false);
  const [formData, setFormData] = useState<UpdateTopicInput>(
    {} as UpdateTopicInput,
  );

  useEffect(() => {
    if (router.isReady) {
      const topicId = router.query.id as string;

      if (!topicId) {
        notFound();
      }
      setTopicId(topicId);
      apiClient.topic.topicDetail(topicId).then((res) => {
        setFormData({
          name: res.data.name,
          description: res.data.description,
          cards: res.data.cards,
        });
      });
    }
  }, [router.isReady]);

  const handleInputChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleFieldChange = (
    keyOrId: string,
    fieldName: string,
    value: string,
  ) => {
    const updatedCards = [...formData.cards!];
    const index = updatedCards.findIndex(
      (x) => x.key === keyOrId || x.id === keyOrId,
    );

    // @ts-ignore
    updatedCards[index][fieldName] = value;
    setFormData((prev) => ({ ...prev, cards: updatedCards }));
  };

  const addCard = () => {
    setFormData((prev) => ({
      ...prev,
      cards: [
        ...prev.cards!,
        {
          id: null,
          key: nanoid(),
          terminology: "",
          definition: "",
        },
      ],
    }));
  };

  const removeCard = (keyOrId: string) => {
    if (formData?.cards) {
      const updatedCards = [...formData.cards];
      const index = updatedCards.findIndex(
        (x) => x.key === keyOrId || x.id === keyOrId,
      );

      if (!updatedCards[index].id) {
        updatedCards.splice(index, 1);
      } else {
        updatedCards[index].isDeleted = true;
      }

      if (updatedCards.filter((x) => !x.isDeleted).length === 0) {
        updatedCards.push({
          id: null,
          key: nanoid(),
          terminology: "",
          definition: "",
        });
      }

      setFormData((prev) => ({ ...prev, cards: updatedCards }));
    }
  };

  const onSubmit = async (e: any) => {
    e.preventDefault();

    try {
      setSubmitted(true);
      const response = await apiClient.topic.topicUpdate(topicId!, formData);

      if (response.status === 200) {
        toast.info("Saved successfully");
        await router.push("/");

        return;
      }

      toast.error("Failed to save");
    } finally {
      setSubmitted(false);
    }
  };

  return (
    <DefaultLayout>
      <Form
        className="w-full space-y-4"
        validationBehavior="native"
        onSubmit={onSubmit}
      >
        <div className="self-end">
          <Button color="primary" isLoading={submitted} type="submit">
            Submit
          </Button>
        </div>
        <div className="w-full flex flex-col gap-4">
          <Input
            isRequired
            errorMessage="Please enter a valid topic name"
            label="Topic name"
            labelPlacement="outside"
            name="name"
            placeholder="Enter the topic name"
            value={formData.name!}
            onChange={(e) => handleInputChange("name", e.target.value)}
          />

          <Textarea
            label="Description"
            labelPlacement="outside"
            name="description"
            placeholder="Enter your description"
            value={formData.description!}
            onChange={(e) => handleInputChange("description", e.target.value)}
          />
        </div>

        {formData.cards
          ?.filter((x) => !x.isDeleted)
          ?.map((card, index) => {
            const key = card.key || card.id || "";

            return (
              <Card key={key} className="w-full p-4">
                <CardHeader className="flex justify-between">
                  <span>{index + 1}</span>
                  <FiTrash
                    className="cursor-pointer"
                    onClick={() => removeCard(key)} // can't use index because we already filter the deleted cards
                  />
                </CardHeader>
                <CardBody className="gap-4">
                  <div className="flex flex-row gap-4">
                    <div className="flex w-1/2">
                      <Input
                        isRequired
                        defaultValue={card.terminology || ""}
                        errorMessage="Required"
                        label="Terminology"
                        name="terminology"
                        placeholder="Enter the terminology"
                        onChange={(e) =>
                          handleFieldChange(key, "terminology", e.target.value)
                        }
                      />
                    </div>
                    <div className="flex w-1/2">
                      <Input
                        isRequired
                        defaultValue={card.definition || ""}
                        errorMessage="Required"
                        label="Definition"
                        name="definition"
                        placeholder="Enter the definition"
                        onChange={(e) =>
                          handleFieldChange(key, "definition", e.target.value)
                        }
                      />
                    </div>
                  </div>
                </CardBody>
              </Card>
            );
          })}

        <Button
          className="self-center"
          color="success"
          type="button"
          onPress={addCard}
        >
          <FiPlus color="#fff" />
        </Button>
      </Form>
    </DefaultLayout>
  );
};

export default TopicEdit;
