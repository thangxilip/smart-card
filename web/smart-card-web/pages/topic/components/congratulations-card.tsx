import Confetti from "react-confetti";
import { useEffect, useState } from "react";
import { Card, CardBody } from "@nextui-org/card";
import { Spacer } from "@nextui-org/spacer";

export default function CongratulationsCard() {
  const [size, setSize] = useState({ width: 0, height: 0 });

  useEffect(() => {
    setSize({ width: window.innerWidth, height: window.innerHeight });
  }, []);

  return (
    <>
      <Confetti height={size.height} recycle={false} width={size.width} />
      <Card className={'w-1/2 h-[50vh] m-auto'}>
        <CardBody className={'flex justify-center items-center'}>
          <h2 className={'text-lg font-medium'}>Congratulation! 🎉</h2>
          <Spacer y={5}/>
          <p className="text-center text-md mb-6">
            You’ve finished the topic for now. Great job!
          </p>
        </CardBody>
      </Card>
    </>
  );
}
