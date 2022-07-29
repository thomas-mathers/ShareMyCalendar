type BinaryOperation = '==';

interface BinaryConstraint {
    op: BinaryOperation;
    lparam: string;
    rparam: string;
}

export default BinaryConstraint;
