import {
  BrowserRouter,
  Switch,
  Route,
  Redirect,
  useHistory,
  useLocation,
  useRouteMatch,
  Link,
} from "react-router-dom";

/**
 * Rexporting the react router components we use under a convenient object.
 */

const Router = {
  Router: BrowserRouter,
  Switch,
  Route,
  Redirect,
  useHistory,
  useLocation,
  useRouteMatch,
  Link,
};

export default Router;
