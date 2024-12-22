import {
  Navbar as NextUINavbar,
  NavbarContent,
  NavbarMenu,
  NavbarMenuToggle,
  NavbarBrand,
  NavbarItem,
  NavbarMenuItem,
} from "@nextui-org/navbar";
import { Button } from "@nextui-org/button";
import { Kbd } from "@nextui-org/kbd";
import { Link } from "@nextui-org/link";
import { Input } from "@nextui-org/input";
import { link as linkStyles } from "@nextui-org/theme";
import NextLink from "next/link";
import clsx from "clsx";
import { FiPlusCircle } from "react-icons/fi";
import { useRouter } from "next/navigation";
import { useDisclosure } from "@nextui-org/modal";
import { useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";
import { Avatar } from "@nextui-org/avatar";
import { Dropdown, DropdownItem, DropdownMenu, DropdownTrigger } from "@nextui-org/dropdown";

import { siteConfig } from "@/config/site";
import { ThemeSwitch } from "@/components/theme-switch";
import { SearchIcon, Logo } from "@/components/icons";
import LoginModal from "@/components/modals/login";
import { UserModel } from "@/api/service-proxy";
import { LOCALSTORAGE_CONSTANTS } from "@/Utils/constants";

export const Navbar = () => {
  const router = useRouter();
  const { isOpen, onOpen, onOpenChange } = useDisclosure();
  const [userInfo, setUserInfo] = useState<UserModel>();

  useEffect(() => {
    const accessToken = localStorage.getItem(
      LOCALSTORAGE_CONSTANTS.ACCESS_TOKEN,
    );

    if (accessToken) {
      const user = jwtDecode(accessToken) as UserModel;

      setUserInfo(user);
    }
  }, []);

  const logout = () => {
    localStorage.removeItem(LOCALSTORAGE_CONSTANTS.ACCESS_TOKEN);
    setUserInfo(undefined);
  };

  const searchInput = (
    <Input
      aria-label="Search"
      classNames={{
        inputWrapper: "bg-default-100",
        input: "text-sm",
      }}
      endContent={
        <Kbd className="hidden lg:inline-block" keys={["command"]}>
          K
        </Kbd>
      }
      labelPlacement="outside"
      placeholder="Search..."
      startContent={
        <SearchIcon className="text-base text-default-400 pointer-events-none flex-shrink-0" />
      }
      type="search"
    />
  );

  return (
    <>
      <NextUINavbar maxWidth="xl" position="sticky">
        <NavbarContent className="basis-1/5 sm:basis-full" justify="start">
          <NavbarBrand className="gap-3 max-w-fit">
            <NextLink
              className="flex justify-start items-center gap-1"
              href="/"
            >
              <Logo />
              <p className="font-bold text-inherit">TV</p>
            </NextLink>
          </NavbarBrand>
          <div className="hidden lg:flex gap-4 justify-start ml-2">
            {siteConfig.navItems.map((item) => (
              <NavbarItem key={item.href}>
                <NextLink
                  className={clsx(
                    linkStyles({ color: "foreground" }),
                    "data-[active=true]:text-primary data-[active=true]:font-medium",
                  )}
                  color="foreground"
                  href={item.href}
                >
                  {item.label}
                </NextLink>
              </NavbarItem>
            ))}
          </div>
        </NavbarContent>

        <NavbarContent
          className="hidden sm:flex basis-1/5 sm:basis-full"
          justify="end"
        >
          <NavbarItem className="hidden lg:flex">{searchInput}</NavbarItem>
          <NavbarItem className="hidden md:flex">
            <Button
              className="text-sm font-normal text-default-600 bg-default-100"
              href="/topic/create"
              startContent={<FiPlusCircle />}
              variant="flat"
              onClick={() => router.push("/topic/create")}
            >
              Create
            </Button>
          </NavbarItem>
          {!userInfo && (
            <NavbarItem className="hidden md:flex">
              <Button color="primary" variant="flat" onClick={onOpen}>
                Sign in
              </Button>
            </NavbarItem>
          )}
          {userInfo && (
            <Dropdown placement="bottom-end">
              <DropdownTrigger>
                <Avatar
                  isBordered
                  as="button"
                  className="transition-transform"
                  color="secondary"
                  name={`${userInfo.firstName![0]}`}
                  size="sm"
                />
              </DropdownTrigger>
              <DropdownMenu aria-label="Profile Actions" variant="flat">
                <DropdownItem key="profile" className="h-14 gap-2">
                  <p className="font-semibold">Signed in as</p>
                  <p className="font-semibold">{userInfo.email}</p>
                </DropdownItem>
                <DropdownItem key="settings">My profile</DropdownItem>
                <DropdownItem key="logout" color="danger" onPress={logout}>
                  Log Out
                </DropdownItem>
              </DropdownMenu>
            </Dropdown>
          )}
        </NavbarContent>

        <NavbarContent className="sm:hidden basis-1 pl-4" justify="end">
          <ThemeSwitch />
          <NavbarMenuToggle />
        </NavbarContent>

        <NavbarMenu>
          {searchInput}
          <div className="mx-4 mt-2 flex flex-col gap-2">
            {siteConfig.navMenuItems.map((item, index) => (
              <NavbarMenuItem key={`${item}-${index}`}>
                <Link
                  color={
                    index === 2
                      ? "primary"
                      : index === siteConfig.navMenuItems.length - 1
                        ? "danger"
                        : "foreground"
                  }
                  href="#"
                  size="lg"
                >
                  {item.label}
                </Link>
              </NavbarMenuItem>
            ))}
          </div>
        </NavbarMenu>
      </NextUINavbar>
      <LoginModal isOpen={isOpen} onOpenChange={onOpenChange} />
    </>
  );
};
