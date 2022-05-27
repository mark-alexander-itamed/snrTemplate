import React from "react";
import ThemeProvider from "../shared/context/ThemeProvider";
import { Container, CssBaseline } from "@material-ui/core";
import Navbar from "../shared/components/Navbar";
import Router from "../shared/utils/Router";
import Routes from "./Routes";
import { AlertProvider } from "../shared/Alerts";

const App = () => {
  return (
    <ThemeProvider>
      <CssBaseline>
        <AlertProvider>
          <Router.Router>
            <Container maxWidth="md">
              <Navbar />
              <Routes />
            </Container>
          </Router.Router>
        </AlertProvider>
      </CssBaseline>
    </ThemeProvider>
  );
};

export default App;
