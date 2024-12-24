import {
  Modal,
  ModalBody,
  ModalContent,
  ModalFooter,
  ModalHeader,
} from "@nextui-org/modal";
import { Button } from "@nextui-org/button";
import { HttpStatusCode } from "axios";
import { Image } from "@nextui-org/image";

import apiClient from "@/api/api-instance";

interface LoginModalProps {
  isOpen: boolean;
  onOpenChange: () => void;
}

const LoginModal = ({ isOpen, onOpenChange }: LoginModalProps) => {
  const loginWithGoogle = async () => {
    const response = await apiClient.auth.googleGenerateLoginUrlList();

    if (response.status === HttpStatusCode.Ok) {
      window.location.href = response.data;
    }
  };

  return (
    <Modal
      className={"p-5"}
      hideCloseButton={true}
      isOpen={isOpen}
      onOpenChange={onOpenChange}
    >
      <ModalContent>
        {() => (
          <>
            <ModalHeader className="flex flex-col gap-1" />
            <ModalBody>
              <Button
                className="flex items-center px-6 py-2 border rounded-full bg-white shadow-md hover:shadow-lg focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-300"
                onPress={loginWithGoogle}
              >
                <Image
                  alt="Google Icon"
                  className="w-5 h-5 mr-3"
                  src="https://www.gstatic.com/firebasejs/ui/2.0.0/images/auth/google.svg"
                />
                <span className="text-gray-700 font-medium">
                  Sign in with Google
                </span>
              </Button>
            </ModalBody>
            <ModalFooter />
          </>
        )}
      </ModalContent>
    </Modal>
  );
};

export default LoginModal;
