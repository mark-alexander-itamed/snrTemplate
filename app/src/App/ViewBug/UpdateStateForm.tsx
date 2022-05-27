import { Field, FieldProps, Form, Formik, FormikHelpers } from "formik";
import React from "react";
import Bug from "../../shared/models/Bug";
import { UpdateBugForm } from "./index";
import {
  Button,
  CardHeader,
  createStyles,
  makeStyles,
  MenuItem,
  TextField,
} from "@material-ui/core";

interface UpdateStateFormProps {
  bug: Bug;
  onSubmit: (form: UpdateBugForm) => Promise<void>;
}

const UpdateStateForm = (props: UpdateStateFormProps) => {
  const classes = useStyles();

  const onSubmit = async (
    form: UpdateBugForm,
    helpers: FormikHelpers<UpdateBugForm>
  ) => {
    await props.onSubmit(form);
    helpers.setSubmitting(false);
  };

  return (
    <Formik<UpdateBugForm>
      initialValues={props.bug}
      onSubmit={onSubmit}
      enableReinitialize
    >
      {(formikProps) => (
        <Form>
          <CardHeader title="Update state" />
          <div className={classes.formRow}>
            <Field name="state">
              {({ field, form }: FieldProps) => (
                <TextField
                  {...field}
                  select
                  className={classes.textField}
                  disabled={form.isSubmitting}
                >
                  <MenuItem value="Open">Open</MenuItem>
                  <MenuItem value="Closed">Closed</MenuItem>
                </TextField>
              )}
            </Field>
            <Button
              color="secondary"
              variant="contained"
              size="small"
              disabled={formikProps.isSubmitting}
              type="submit"
            >
              Ok
            </Button>
          </div>
        </Form>
      )}
    </Formik>
  );
};

export default UpdateStateForm;

const useStyles = makeStyles((theme) =>
  createStyles({
    formRow: {
      display: "flex",
      padding: theme.spacing(1),
    },
    textField: {
      flex: "1 1 auto",
      marginRight: theme.spacing(1),
    },
  })
);
