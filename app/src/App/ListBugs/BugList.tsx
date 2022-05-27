import React from "react";
import Bug from "../../shared/models/Bug";
import {
  Button,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@material-ui/core";
import Router from "../../shared/utils/Router";
import DateUtils from "../../shared/utils/DateUtils";
import Skeleton from "@material-ui/lab/Skeleton";

interface BugListProps {
  bugs: Bug[] | null;
  isLoading: boolean;
}

const BugList = (props: BugListProps) => {
  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell />
            <TableCell>Title</TableCell>
            <TableCell>State</TableCell>
            <TableCell>Reporter</TableCell>
            <TableCell>Reported date</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {props.bugs?.map((bug) => (
            <TableRow key={bug.id}>
              <TableCell width={80}>
                <Button
                  color="primary"
                  variant="contained"
                  component={Router.Link}
                  to={`/bug/${bug.id}`}
                  size="small"
                >
                  More
                </Button>
              </TableCell>
              <TableCell>{bug.title}</TableCell>
              <TableCell>{bug.state}</TableCell>
              <TableCell>{bug.reportedBy}</TableCell>
              <TableCell>{formatDate(bug.reportedAt)}</TableCell>
            </TableRow>
          )) ?? <BugDataSkeleton />}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

const formatDate = (isoDate: string) => {
  const date = new Date(isoDate);
  return DateUtils.format(date, "HH:mm dd/MM/yyyy");
};

export default BugList;

const BugDataSkeleton = () => (
  <>
    {[0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map((i) => (
      <TableRow key={i}>
        <TableCell width={80}>
          <Skeleton variant="rect" height={30} width={64} />
        </TableCell>
        <TableCell>
          <Skeleton variant="text" />
        </TableCell>
        <TableCell>
          <Skeleton variant="text" />
        </TableCell>
        <TableCell>
          <Skeleton variant="text" />
        </TableCell>
        <TableCell>
          <Skeleton variant="text" />
        </TableCell>
      </TableRow>
    ))}
  </>
);
