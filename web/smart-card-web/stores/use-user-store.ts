import { create } from "zustand/react";
import { persist } from "zustand/middleware";

import { UserModel } from "@/api/service-proxy";
import { LOCALSTORAGE_CONSTANTS } from "@/utils/constants";

interface UserStore {
  user: UserModel | null;
  setUser: (user: UserModel) => void;
  clearUser: () => void;
}

const useUserStore = create<UserStore>()(
  persist(
    (set) => ({
      user: null,

      // actions
      setUser: (user: UserModel) => set({ user }),
      clearUser: () => set({ user: null }),
    }),
    {
      name: LOCALSTORAGE_CONSTANTS.USER_INFO,
    },
  ),
);

export default useUserStore;
