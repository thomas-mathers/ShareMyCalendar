export enum ActionType {
    FieldChanged
}

interface FieldChangedPayload {
    name: string;
    value: string;
}

interface FieldChangedAction {
    type: ActionType.FieldChanged;
    payload: FieldChangedPayload;
}

type Action = FieldChangedAction;

export type { Action };
