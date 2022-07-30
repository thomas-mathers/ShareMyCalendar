import { Field } from './fields';

type BinaryOperation = '==';

interface BinaryConstraint {
    op: BinaryOperation;
    lparam: string;
    rparam: string;
}

function areConstraintsSatisfied(fields: Record<string, Field>, constraints: BinaryConstraint[]) {
    for (const constraint of constraints) {
        const lparam = fields[constraint.lparam];
        const rparam = fields[constraint.rparam];
        switch (constraint.op) {
            case '==':
                if (lparam.value !== rparam.value) {
                    return false;
                }
        }
    }
    return true;
}

export default BinaryConstraint;
export { areConstraintsSatisfied }