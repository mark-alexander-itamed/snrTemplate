import React from "react";
import {
  AppBar,
  Button,
  createStyles,
  makeStyles,
  Toolbar,
  Typography,
} from "@material-ui/core";
import Router from "../utils/Router";

interface NavbarProps {}

const Navbar = (props: NavbarProps) => {
  const classes = useStyles();
  const location = Router.useLocation();
  return (
    <AppBar position="static" className={classes.appBar} elevation={0}>
      <Toolbar>
        <Typography variant="h6">G&W Bugs</Typography>
        <div className={classes.navigationButtons}>
          {location.pathname !== "/" && (
            <Button component={Router.Link} to="/" variant="contained">
              Home
            </Button>
          )}
          {!location.pathname.startsWith("/report") && (
            <Button
              component={Router.Link}
              to="/report"
              color="secondary"
              variant="contained"
            >
              Report a bug
            </Button>
          )}
        </div>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;

const useStyles = makeStyles((theme) =>
  createStyles({
    appBar: {
      margin: theme.spacing(1, 0),
    },
    navigationButtons: {
      marginLeft: "auto",
      display: "flex",
      "&>*:not(:last-child)": {
        marginRight: theme.spacing(1),
      },
    },
    reportButton: {
      marginLeft: "auto",
    },
  })
);
