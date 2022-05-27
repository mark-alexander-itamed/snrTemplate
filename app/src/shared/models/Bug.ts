import { BugComment } from "./BugComment";

export default interface Bug {
  id: string;
  title: string;
  description: string;
  reportedBy: string;
  reportedAt: string;
  state: "Open" | "Closed";
  comments: BugComment[];
}
