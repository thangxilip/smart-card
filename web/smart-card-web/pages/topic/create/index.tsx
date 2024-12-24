"use client";

import { Form, Input, Button, Textarea } from "@nextui-org/react";
import { useState } from "react";
import { Card, CardBody, CardHeader } from "@nextui-org/card";
import { FiTrash, FiPlus } from "react-icons/fi";
import { toast } from "react-toastify";
import { useRouter } from "next/navigation";
import { nanoid } from "nanoid";

import DefaultLayout from "@/layouts/default";
import { CreateTopicInput, BaseCardDto } from "@/api/service-proxy";
import apiClient from "@/api/api-instance";

const TopicCreate = () => {
  const router = useRouter();

  const [submited, setSubmited] = useState(false);
  const [formData, setFormData] = useState<CreateTopicInput>({
    name: "",
    description: "",
    cards: [
      {
        key: nanoid(),
        terminology: "",
        definition: "",
      },
    ] as BaseCardDto[],
  });

  const handleInputChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleFieldChange = (index: number, field: string, value: string) => {
    const updatedCards: BaseCardDto[] = [...formData.cards!];

    // @ts-ignore
    updatedCards[index][field] = value;
    setFormData((prev) => ({ ...prev, cards: updatedCards }));
  };

  // Add a new dynamic field
  const addCard = () => {
    setFormData((prev) => ({
      ...prev,
      cards: [
        ...prev.cards!,
        {
          key: nanoid(),
          terminology: "",
          definition: "",
        } as BaseCardDto,
      ],
    }));
  };

  // Remove a dynamic field
  const removeCard = (index: number) => {
    const updatedCards = formData.cards!.filter((_, i: number) => i !== index);

    setFormData((prev) => ({ ...prev, cards: updatedCards }));
  };

  const onSubmit = async (e: any) => {
    e.preventDefault();
    try {
      setSubmited(true);
      const response = await apiClient.topic.topicCreate(formData);

      if (response.status === 200) {
        toast.info("Saved successfully");
        await router.push("/");

        return;
      }

      toast.error("Failed to save");
    } finally {
      setSubmited(false);
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
          <Button color="primary" isLoading={submited} type="submit">
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

        {formData.cards!.map((card, index) => (
          <Card key={card.key} className="w-full p-4">
            <CardHeader className="flex justify-between">
              <span>{index + 1}</span>
              <FiTrash
                className="cursor-pointer"
                onClick={() => removeCard(index)}
              />
            </CardHeader>
            <CardBody className="gap-4">
              <div className="flex flex-row gap-4">
                <div className="flex w-1/2">
                  <Input
                    isRequired
                    errorMessage="Required"
                    label="Terminology"
                    name="terminology"
                    placeholder="Enter the terminology"
                    onChange={(e) =>
                      handleFieldChange(index, "terminology", e.target.value)
                    }
                  />
                </div>
                <div className="flex w-1/2">
                  <Input
                    isRequired
                    errorMessage="Required"
                    label="Definition"
                    name="definition"
                    placeholder="Enter the definition"
                    onChange={(e) =>
                      handleFieldChange(index, "definition", e.target.value)
                    }
                  />
                </div>
              </div>
            </CardBody>
          </Card>
        ))}

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

export default TopicCreate;
