import React from "react";
import { createStyles, makeStyles, Snackbar } from "@material-ui/core";
import { Alert as MuiAlert, AlertTitle } from "@material-ui/lab";

type AlertSeverity = "info" | "warning" | "success" | "error";

interface Alert {
  title?: string;
  message: string;
  timeoutHandle: number;
  severity: AlertSeverity;
}

interface AlertContextValue {
  alert: (
    message: string,
    title?: string,
    severity?: AlertSeverity,
    timeoutDuration?: number
  ) => void;
}

const AlertContext = React.createContext<AlertContextValue>({
  alert: () => {},
});

export const AlertProvider = (props: React.PropsWithChildren<{}>) => {
  const classes = useStyles();

  const [alert, setAlert] = React.useState<Alert | null>(null);

  const safeCreateAlert = (alert: Alert) =>
    setAlert((prev) => {
      if (prev) {
        console.log("alert removed");
        window.clearTimeout(prev.timeoutHandle);
      }

      console.log("alert created");
      return alert;
    });

  React.useEffect(() => {
    return () =>
      setAlert((alert) => {
        if (alert) {
          window.clearTimeout(alert.timeoutHandle);
        }
        return alert;
      });
  }, [setAlert]);

  const createAlert = (
    message: string,
    title?: string,
    severity: AlertSeverity = "info",
    timeoutDuration: number = 5000
  ) =>
    safeCreateAlert({
      message,
      title,
      severity,
      timeoutHandle: window.setTimeout(() => {
        setAlert(null);
      }, timeoutDuration),
    });

  return (
    <AlertContext.Provider value={{ alert: createAlert }}>
      <div className={classes.root}>
        {alert && (
          <div className={classes.alertContainer}>
            <MuiAlert severity={alert.severity}>
              {alert.title && <AlertTitle>{alert.title}</AlertTitle>}
              <span>{alert.message}</span>
            </MuiAlert>
          </div>
        )}
      </div>
      {props.children}
    </AlertContext.Provider>
  );
};

export const useAlert = () => React.useContext(AlertContext);

const useStyles = makeStyles((theme) =>
  createStyles({
    root: {
      position: "absolute",
      top: 0,
      right: 0,
      bottom: 0,
      left: 0,
    },
    alertContainer: {
      position: "absolute",
      top: 20,
      right: 20,
      width: 400,
    },
  })
);
