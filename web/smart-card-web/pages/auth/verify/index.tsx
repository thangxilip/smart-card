import { useRouter } from "next/router";
import { useEffect } from "react";
import { HttpStatusCode } from "axios";
import { jwtDecode } from "jwt-decode";

import apiClient from "@/api/api-instance";
import { LOCALSTORAGE_CONSTANTS } from "@/Utils/constants";
import { UserModel } from "@/api/service-proxy";
import useUserStore from "@/stores/use-user-store";

const VerifyLoginPage = () => {
  const router = useRouter();
  const { setUser } = useUserStore();

  const setUserInfo = () => {
    const accessToken = localStorage.getItem(
      LOCALSTORAGE_CONSTANTS.ACCESS_TOKEN,
    );

    if (accessToken) {
      const user = jwtDecode(accessToken!) as UserModel;

      setUser(user);
    }
  };

  useEffect(() => {
    if (router.isReady) {
      const code = router.query.code as string;

      if (code) {
        apiClient.auth.googleExchangeCodeList({ code }).then((res) => {
          if (res.status === HttpStatusCode.Ok) {
            localStorage.setItem(
              LOCALSTORAGE_CONSTANTS.ACCESS_TOKEN,
              res.data.accessToken!,
            );
            setUserInfo();
            router.push("/");
          }
        });
      }
    }
  }, [router.isReady]);

  return <div>Authenticating...</div>;
};

export default VerifyLoginPage;
