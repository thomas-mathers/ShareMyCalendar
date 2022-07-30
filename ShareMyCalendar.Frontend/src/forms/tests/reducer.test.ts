import { Action, ActionType } from "../actions";
import { State } from "../state";
import reducer from "../reducer"
import { FieldType } from "../fields";
import requiresMinLength from "../validators/requires-min-length";
import requiresLower from "../validators/requires-lower";
import requiresUpper from "../validators/requires-upper";
import requiresDigit from "../validators/requires-digit";
import requiresNonAlpha from "../validators/requires-non-alpha";

test('correctly removes all validation messages to valid input', () => {
  const state: State = {
    fields: {
      password: {
        type: FieldType.Text,
        label: 'Password',
        value: '',
        validators: [
          requiresMinLength(6),
          requiresLower,
          requiresUpper,
          requiresDigit,
          requiresNonAlpha,
        ],
        errorMessages: [
          'Password must be at least 6 characters long',
          'Password requires a lowercase letter',
          'Password requires an uppercase letter',
          'Password requires a digit',
          'Password requires a non alphanumeric character'
        ],
      }
    },
    constraints: [],
  };
  const action: Action = {
    type: ActionType.FieldChanged,
    payload: {
      name: 'password',
      value: 'Freedom$8'
    }
  }

  // Act
  const actual = reducer(state, action);

  // Assert
  expect(actual.fields.password.errorMessages).toEqual([]);
})

test('correctly adds all validation messages to invalid input', () => {
  const state: State = {
    fields: {
      password: {
        type: FieldType.Text,
        label: 'Password',
        value: 'Freedom$8',
        validators: [
          requiresMinLength(6),
          requiresLower,
          requiresUpper,
          requiresDigit,
          requiresNonAlpha,
        ],
        errorMessages: [],
      }
    },
    constraints: [],
  };
  const action: Action = {
    type: ActionType.FieldChanged,
    payload: {
      name: 'password',
      value: ''
    }
  }

  // Act
  const actual = reducer(state, action);

  // Assert
  expect(actual.fields.password.errorMessages).toEqual([
    'Password must be at least 6 characters long',
    'Password requires a lowercase letter',
    'Password requires an uppercase letter',
    'Password requires a digit',
    'Password requires a non alphanumeric character'
  ]);
})

it.each([
  ['password', 'Freedom$7'],
  ['confirmPassword', 'Freedom$8']
])('correctly removes validation messages from both fields with matching constraint when change is made to make them matching', (name: string, value: string) => {
  // Arrange
  const state: State = {
    fields: {
      password: {
        type: FieldType.Text,
        label: 'Password',
        value: 'Freedom$8',
        validators: [],
        errorMessages: [
          'Password must match Confirm Password'
        ],
      },
      confirmPassword: {
        type: FieldType.Text,
        label: 'Confirm Password',
        value: 'Freedom$7',
        validators: [],
        errorMessages: [
          'Confirm Password must match Password'
        ]
      }
    },
    constraints: [
      { op: '==', lparam: 'password', rparam: 'confirmPassword' }
    ]
  };
  const action: Action = {
    type: ActionType.FieldChanged,
    payload: {
      name: name,
      value: value,
    }
  }

  // Act
  const actual = reducer(state, action);

  // Assert
  expect(actual.fields.password.errorMessages).toEqual([]);
  expect(actual.fields.confirmPassword.errorMessages).toEqual([]);
});

it.each([
  ['password'],
  ['confirmPassword']
])('correctly adds validation message to both fields with matching constraint when change is made which makes them not matching', (name: string) => {
  // Arrange
  const state: State = {
    fields: {
      password: {
        type: FieldType.Text,
        label: 'Password',
        value: 'Freedom$8',
        validators: [],
        errorMessages: [],
      },
      confirmPassword: {
        type: FieldType.Text,
        label: 'Confirm Password',
        value: 'Freedom$8',
        validators: [],
        errorMessages: []
      }
    },
    constraints: [
      { op: '==', lparam: 'password', rparam: 'confirmPassword' }
    ]
  };
  const action: Action = {
    type: ActionType.FieldChanged,
    payload: {
      name: name,
      value: 'Freedom$7',
    }
  }

  // Act
  const actual = reducer(state, action);

  // Assert
  expect(actual.fields.password.errorMessages).toEqual(['Password must equal Confirm Password']);
  expect(actual.fields.confirmPassword.errorMessages).toEqual(['Confirm Password must equal Password']);
});