import React from "react";
import ReportForm from "./ReportForm";
import { object, string } from "yup";
import { Formik, FormikHelpers } from "formik";
import useMutation from "../../shared/hooks/useMutation";
import Bug from "../../shared/models/Bug";
import Router from "../../shared/utils/Router";

interface ReportBugProps {}

export interface ReportForm {
  title: string;
  description: string;
  reportedBy: string;
}

const ValidationSchema = object().shape({
  title: string()
    .required("A title is required")
    .max(256, "Max length is 256 characters"),
  description: string()
    .required("A description is required")
    .max(2048, "Max length is 2048 characters"),
  reportedBy: string()
    .required("Your name is required")
    .max(128, "Max length is 256 characters"),
});

const ReportBug = (props: ReportBugProps) => {
  const history = Router.useHistory();
  const [submitRequest, submitAction] = useMutation<Bug>("post", "/api/bug");

  const onSubmit = async (
    form: ReportForm,
    helpers: FormikHelpers<ReportForm>
  ) => {
    try {
      const response = await submitAction.request(form);

      history.push(`/bug/${response.id}`);
    } catch (e) {
      console.error(e);
      helpers.setSubmitting(false);
    }
  };

  return (
    <Formik<ReportForm>
      initialValues={{
        title: "",
        description: "",
        reportedBy: "",
      }}
      onSubmit={onSubmit}
      validateOnMount
      validationSchema={ValidationSchema}
    >
      {(formikProps) => (
        <ReportForm locked={formikProps.isSubmitting || !formikProps.isValid} />
      )}
    </Formik>
  );
};

export default ReportBug;
