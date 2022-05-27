import React from "react";
import Router from "../shared/utils/Router";
import ListBugs from "./ListBugs";
import ReportBug from "./ReportBug";
import ViewBug from "./ViewBug";

interface RoutesProps {}

const Routes = (props: RoutesProps) => {
  return (
    <Router.Switch>
      <Router.Route path="/" exact>
        <ListBugs />
      </Router.Route>
      <Router.Route path="/report">
        <ReportBug />
      </Router.Route>
      <Router.Route
        path="/bug/:bugId"
        render={(props) => <ViewBug bugId={props.match.params.bugId} />}
      />
      <Router.Redirect to="/" />
    </Router.Switch>
  );
};

export default Routes;
