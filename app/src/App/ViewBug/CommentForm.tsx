import React from "react";
import { AddCommentForm } from "./index";
import { object, string } from "yup";
import { Field, FieldProps, Form, Formik, FormikHelpers } from "formik";
import {
  Button,
  Card,
  CardActions,
  CardContent,
  CardHeader,
  createStyles,
  Fade,
  makeStyles,
  TextField,
} from "@material-ui/core";
import { useAlert } from "../../shared/Alerts";
import { AxiosError } from "axios";
import isAxiosError from "../../shared/utils/isAxiosError";

interface CommentFormProps {
  onSubmit: (form: AddCommentForm) => Promise<void>;
  locked: boolean;
}

const ValidationSchema = object().shape({
  text: string()
    .required("Some text is required")
    .max(2048, "Max 2048 characters"),
});

const CommentForm = (props: CommentFormProps) => {
  const classes = useStyles();
  const alert = useAlert();
  const onSubmit = async (
    form: AddCommentForm,
    helpers: FormikHelpers<AddCommentForm>
  ) => {
    try {
      await props.onSubmit(form);
      helpers.resetForm();
    } catch (e) {
      if (isAxiosError(e)) {
        alert.alert(
          e.response?.data.message,
          e.message,
          //"An error occurred saving your comment",
          "error"
        );
      }
      helpers.setSubmitting(false);
    }
  };

  return (
    <Formik<AddCommentForm>
      initialValues={{
        text: "",
      }}
      onSubmit={onSubmit}
      validationSchema={ValidationSchema}
    >
      {(formikProps) => (
        <Form className={classes.card}>
          <Fade in={!props.locked} timeout={200}>
            <Card variant="outlined">
              <CardHeader title="Add a comment" />
              <CardContent className={classes.form}>
                <Field name="text">
                  {({ field, form, meta }: FieldProps) => (
                    <TextField
                      {...field}
                      disabled={form.isSubmitting || props.locked}
                      label="Comment"
                      helperText={(meta.touched && meta.error) || " "}
                      multiline
                      rows={5}
                      rowsMax={15}
                      inputProps={{ maxLength: 2048 }}
                      error={meta.touched && !!meta.error}
                    />
                  )}
                </Field>
                <div className={classes.actions}>
                  <Button
                    type="submit"
                    variant="contained"
                    color="primary"
                    disabled={
                      formikProps.isSubmitting ||
                      props.locked ||
                      !formikProps.isValid
                    }
                  >
                    Submit
                  </Button>
                </div>
              </CardContent>
            </Card>
          </Fade>
        </Form>
      )}
    </Formik>
  );
};

export default CommentForm;

const useStyles = makeStyles((theme) =>
  createStyles({
    card: {
      marginTop: theme.spacing(5),
    },
    form: {
      "&>*:not(:last-child)": {
        marginBottom: theme.spacing(2),
      },
    },
    actions: {
      display: "flex",
      justifyContent: "flex-end",
      "&>*:not(:last-child)": {
        marginRight: theme.spacing(1),
      },
    },
  })
);
