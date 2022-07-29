import { Field } from './fields';
import BinaryConstraint from "./binary-constraint";

export interface State {
    fields: Record<string, Field>;
    constraints: BinaryConstraint[];
}
