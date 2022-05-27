import { createMuiTheme, MuiThemeProvider } from "@material-ui/core";
import * as colors from "@material-ui/core/colors";
import React from "react";

export const MaterialTheme = createMuiTheme({
  palette: {
    primary: colors.blueGrey,
    secondary: colors.orange,
  },
  typography: {
    fontFamily:
      '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif, "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol";',
  },
  overrides: {
    MuiButton: {
      label: {
        textTransform: "capitalize",
      },
    },
    MuiScopedCssBaseline: {
      root: {
        "& a": {
          textDecoration: "none",
        },
      },
    },
  },
  props: {
    MuiTextField: {
      variant: "outlined",
      size: "small",
      fullWidth: true,
    },
    MuiButton: {
      disableElevation: true,
    },
  },
});

if (process.env.NODE_ENV === "development") {
  /*
   * This makes the theme object interactive through the console.
   * In the browser, open the console and type 'theme', and you'll
   * be able to browse the theme.
   */

  // @ts-ignore
  window.theme = MaterialTheme;
}

const ThemeProvider = ({ children }: React.PropsWithChildren<{}>) => (
  <MuiThemeProvider theme={MaterialTheme}>{children}</MuiThemeProvider>
);

export default ThemeProvider;
