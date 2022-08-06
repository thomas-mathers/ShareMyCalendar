import BinaryConstraint, { areConstraintsSatisfied } from "../binary-constraint";
import { Field, FieldType } from "../fields";

it.each([
    ['a', 'a', true],
    ['thomas mathers', 'thomas mathers', true],
    ['a', 'b', false],
    ['thomas mathers', 'thomas smith', false]
])('returns correct value', (value1: string, value2: string, areEqual: boolean) => {
    // Arrange
    const constraint: BinaryConstraint = { op: '==', lparam: 'variable1', rparam: 'variable2' };
    const constraints = [constraint];
    const fields: Record<string, Field> = {
        'variable1': {
            type: FieldType.Text,
            label: 'variable1',
            value: value1,
            validators: [],
            errorMessages: [],
        },
        'variable2': {
            type: FieldType.Text,
            label: 'variable2',
            value: value2,
            validators: [],
            errorMessages: [],
        }
    }

    // Act
    const actual = areConstraintsSatisfied(fields, constraints);

    // Assert
    expect(actual).toBe(areEqual);
});