import { useRouter } from "next/router";
import { useEffect } from "react";
import { HttpStatusCode } from "axios";

import apiClient from "@/api/api-instance";
import { LOCALSTORAGE_CONSTANTS } from "@/Utils/constants";

const VerifyLoginPage = () => {
  const router = useRouter();

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
            router.push("/");
          }
        });
      }
    }
  }, [router.isReady]);

  return <div>Authenticating...</div>;
};

export default VerifyLoginPage;
