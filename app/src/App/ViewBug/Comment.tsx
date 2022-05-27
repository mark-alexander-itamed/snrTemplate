import React from "react";
import { BugComment } from "../../shared/models/BugComment";
import DateUtils from "../../shared/utils/DateUtils";

interface BugCommentProps {
  comment: BugComment;
}

const Comment = (props: BugCommentProps) => {
  /**
   * Make this look prettier!
   *
   * Use Material UI components. You can see these in action throughout this
   * project, or from the documentation at: https://material-ui.com/
   *
   * For the date functions, look at the docs for date-fns: https://date-fns.org/docs/Getting-Started
   *
   * If you want to use any of those functions, use 'DateUtils'
   *
   * For example, `const date = DateUtils.parseIso(props.comment.commentedAt)` will parse the date
   * from the comment.
   *
   * What we'd like to see:
   * - The name of the person who made the comment.
   * - The comment text.
   * - A reference to when the comment was made. Try to make this state the period of time
   *   since the comment was made, like '2 hours ago'. There should be a function in date-fns
   *   that'll help.
   */

  return (
    <div>
      {props.comment.text} - <em>{props.comment.commentedAt}</em>
    </div>
  );
};

export default Comment;
