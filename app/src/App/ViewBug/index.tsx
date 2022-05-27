import React from "react";
import { useQuery } from "../../shared/hooks/useQuery";
import Bug from "../../shared/models/Bug";
import BugInfo from "./BugInfo";
import useMutation from "../../shared/hooks/useMutation";
import { BugComment } from "../../shared/models/BugComment";

interface ViewBugProps {
  bugId: string;
}

export type UpdateBugForm = Pick<Bug, "state" | "title" | "description">;

export type AddCommentForm = Pick<BugComment, "text">;

const ViewBug = (props: ViewBugProps) => {
  const [bugRequest, bugAction] = useQuery<Bug>(`/api/bug/${props.bugId}`);
  const [updateStateRequest, updateStateAction] = useMutation<Bug>(
    "patch",
    `/api/bug/${props.bugId}`
  );

  const [addCommentRequest, addCommentAction] = useMutation<Bug>(
    "post",
    `/api/bug/${props.bugId}/comment`
  );

  React.useEffect(() => {
    bugAction.request().then();
  }, []);

  const updateState = async (form: UpdateBugForm) => {
    let rollback: (() => void) | null = null;
    try {
      const response = await updateStateAction.request(form);
      rollback = bugAction.optimisticUpdate(response);
      await bugAction.request();
    } catch (e) {
      console.error(e);
      rollback?.();
    }
    return;
  };

  const addComment = async (form: AddCommentForm) => {
    let rollback: (() => void) | null = null;
    try {
      const response = await addCommentAction.request(form);
      rollback = bugAction.optimisticUpdate(response);
      await bugAction.request();
    } catch (e) {
      console.error(e);
      rollback?.();
      throw e;
    }

    return;
  };

  return (
    <BugInfo
      bug={bugRequest.data}
      loading={bugRequest.loading}
      updateState={updateState}
      addComment={addComment}
    />
  );
};

export default ViewBug;
