import {
  Avatar,
  Card,
  CardHeader,
  createStyles,
  Grid,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  makeStyles,
  Paper,
  Typography,
} from "@material-ui/core";
import React from "react";
import Bug from "../../shared/models/Bug";
import DateUtils from "../../shared/utils/DateUtils";
import { faUser } from "@fortawesome/free-solid-svg-icons/faUser";
import { faCalendar } from "@fortawesome/free-solid-svg-icons/faCalendar";
import { faClipboardList } from "@fortawesome/free-solid-svg-icons/faClipboardList";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { AddCommentForm, UpdateBugForm } from "./index";
import UpdateStateForm from "./UpdateStateForm";
import Skeleton from "@material-ui/lab/Skeleton";
import BugComment from "./Comment";
import CommentForm from "./CommentForm";

interface BugInfoProps {
  bug: Bug | null;
  loading: boolean;
  updateState: (form: UpdateBugForm) => Promise<void>;
  addComment: (form: AddCommentForm) => Promise<void>;
}

const BugInfo = (props: BugInfoProps) => {
  const classes = useStyles();
  return (
    <Paper className={classes.paper}>
      <Grid spacing={2} container>
        <Grid item xs={12} md={4}>
          <Card variant="outlined">
            <CardHeader title="Info" />
            <List disablePadding>
              {[
                {
                  prop: (bug: Bug) => bug.reportedBy,
                  label: "Reporter",
                  icon: faUser,
                },
                {
                  prop: (bug: Bug) =>
                    DateUtils.format(
                      new Date(bug.reportedAt),
                      "HH:mm dd/MM/yyyy"
                    ),
                  label: "Reported at",
                  icon: faCalendar,
                },
                {
                  prop: (bug: Bug) => bug.state,
                  label: "Current state",
                  icon: faClipboardList,
                },
              ].map(({ prop, label, icon }, idx) => (
                <ListItem key={idx} dense>
                  <ListItemAvatar>
                    <Avatar className={classes.avatar}>
                      <FontAwesomeIcon icon={icon} />
                    </Avatar>
                  </ListItemAvatar>
                  <ListItemText
                    primary={
                      props.bug ? prop(props.bug) : <Skeleton variant="text" />
                    }
                    secondary={props.bug ? label : <Skeleton variant="text" />}
                  />
                </ListItem>
              ))}
            </List>
            {!!props.bug ? (
              <UpdateStateForm bug={props.bug} onSubmit={props.updateState} />
            ) : (
              <Skeleton variant="rect" height={120} />
            )}
          </Card>
        </Grid>
        <Grid item xs={12} md={8}>
          <Typography variant="h5" className={classes.bugTitle}>
            {props.bug?.title ?? <Skeleton variant="text" />}
          </Typography>
          <Typography>
            {props.bug?.description ?? (
              <>
                <Skeleton variant="text" />
                <Skeleton variant="text" />
                <Skeleton variant="text" />
                <Skeleton variant="text" />
              </>
            )}
          </Typography>
          {(props.bug?.comments?.length ?? 0) > 0 && (
            <div className={classes.commentsSection}>
              <Typography variant="h6">Comments</Typography>
              {props.bug?.comments?.map((c) => (
                <BugComment key={c.id} comment={c} />
              ))}
            </div>
          )}
          <CommentForm onSubmit={props.addComment} locked={!props.bug} />
        </Grid>
      </Grid>
    </Paper>
  );
};

export default BugInfo;

const useStyles = makeStyles((theme) =>
  createStyles({
    paper: {
      marginTop: theme.spacing(2),
      padding: theme.spacing(2, 4),
    },
    avatar: {
      backgroundColor: theme.palette.common.black,
      color: theme.palette.secondary.main,
    },
    bugTitle: {
      marginBottom: theme.spacing(1),
    },
    commentsSection: {
      marginTop: theme.spacing(2),
    },
  })
);
