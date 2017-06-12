export class ValidationError {
    field: string;
    message: string;
}

export class ErrorResponse {
    message: string;
    errors: ValidationError[];
}