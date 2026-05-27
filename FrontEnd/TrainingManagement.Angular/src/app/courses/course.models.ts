export interface Course {
  courseId: number;
  courseName: string;
  courseDescription: string;
  duration: number;
}

export interface CourseForm {
  courseId: number | null;
  courseName: string;
  courseDescription: string;
  duration: number;
}

export interface RequestDto<T> {
  url: string;
  method: string;
  data: T;
}

export interface ResponseDto<T> {
  data: T | null;
  isRequestProcessed: boolean;
  message: string;
  statusCode: number;
  errors: string[] | null;
}
