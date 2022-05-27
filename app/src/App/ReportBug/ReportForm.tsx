import React from "react";
import {
  Button,
  createStyles,
  makeStyles,
  Paper,
  TextField,
  Typography,
} from "@material-ui/core";
import { Field, FieldProps, Form } from "formik";

interface ReportFormProps {
  locked: boolean;
}

const ReportForm = (props: ReportFormProps) => {
  const classes = useStyles();

  return (
    <Paper className={classes.paper}>
      <Typography variant="h5" className={classes.heading}>
        Report a bug
      </Typography>
      <Typography>
        Use this form to report a new bug. Please fill in the details below, and
        the support team will respond as soon as is possible.
      </Typography>
      <Form className={classes.form}>
        <Field name="title">
          {({ field, form, meta }: FieldProps) => (
            <TextField
              {...field}
              label="Title"
              error={meta.touched && !!meta.error}
              disabled={form.isSubmitting}
              helperText={
                (meta.touched && meta.error) ||
                "A short description of the issue you're having"
              }
              inputProps={{
                maxLength: 256,
              }}
            />
          )}
        </Field>
        <Field name="description">
          {({ field, form, meta }: FieldProps) => (
            <TextField
              {...field}
              label="Description"
              error={meta.touched && !!meta.error}
              disabled={form.isSubmitting}
              helperText={
                (meta.touched && meta.error) ||
                "Please provide more details about this issue"
              }
              inputProps={{
                maxLength: 2048,
              }}
              multiline
              rows={5}
              rowsMax={15}
            />
          )}
        </Field>
        <Field name="reportedBy">
          {({ field, form, meta }: FieldProps) => (
            <TextField
              {...field}
              label="Your name"
              error={meta.touched && !!meta.error}
              disabled={form.isSubmitting}
              helperText={
                (meta.touched && meta.error) || "Enter your name here"
              }
              inputProps={{
                maxLength: 128,
              }}
            />
          )}
        </Field>
        <div className={classes.buttons}>
          <Button
            color="primary"
            variant="contained"
            type="submit"
            disabled={props.locked}
          >
            Submit
          </Button>
        </div>
      </Form>
    </Paper>
  );
};

export default ReportForm;

const useStyles = makeStyles((theme) =>
  createStyles({
    paper: {
      padding: theme.spacing(2, 4),
    },
    heading: {
      marginBottom: theme.spacing(1),
    },
    form: {
      marginTop: theme.spacing(2),
      "&>*:not(:last-child)": {
        marginBottom: theme.spacing(2),
      },
    },
    buttons: {
      display: "flex",
      justifyContent: "flex-end",
      "&>*:not(:last-child)": {
        marginRight: theme.spacing(1),
      },
    },
  })
);
