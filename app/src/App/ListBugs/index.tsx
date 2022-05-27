import React from "react";
import { useQuery } from "../../shared/hooks/useQuery";
import Bug from "../../shared/models/Bug";
import BugList from "./BugList";

const ListBugs = () => {
  const [bugsRequest, bugsAction] = useQuery<Bug[]>("/api/bug");

  React.useEffect(() => {
    bugsAction.request().then();
  }, []);

  return <BugList bugs={bugsRequest.data} isLoading={bugsRequest.loading} />;
};

export default ListBugs;
