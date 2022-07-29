import { Action, ActionType } from "./actions";
import { State } from "./state";
import { Field } from "./fields";
import BinaryConstraint from "./binary-constraint";

function validate(field: Field) {
    field.errorMessages = [];

    for (let i = 0; i < field.validators.length; i++) {
        const errorMessage = field.validators[i](field);

        if (errorMessage) {
            field.errorMessages.push(errorMessage);
        }
    }
}

function isValid(field: Field) {
    for (let i = 0; i < field.validators.length; i++) {
        const errorMessage = field.validators[i](field);

        if (errorMessage) {
            return false;
        }
    }
    return true;
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

function reducer(state: State, action: Action): State {
    const { fields: currentFields, constraints } = state;
    const { type } = action;
    switch (type) {
        case ActionType.FieldChanged:
            const { payload } = action;
            const { name, value } = payload;
            const fields = { ...currentFields };

            const field = fields[name];

            field.value = value;

            validate(field);

            for (const constraint of constraints) {
                if (name !== constraint.lparam && name !== constraint.rparam) {
                    continue;
                }

                const lparam = fields[constraint.lparam];
                const rparam = fields[constraint.rparam];

                if (name === constraint.lparam) {
                    validate(rparam);
                } else {
                    validate(lparam);
                }

                switch (constraint.op) {
                    case '==':
                        if (lparam.value !== rparam.value) {
                            lparam.errorMessages.push(`${lparam.label} must equal ${rparam.label}`);
                            rparam.errorMessages.push(`${rparam.label} must equal ${lparam.label}`);
                        }
                        break;
                }
            }

            return { ...state, fields };
        default:
            throw new Error(`Unrecognized action: ${action.type}`);
    }
}

export { isValid, areConstraintsSatisfied, reducer }