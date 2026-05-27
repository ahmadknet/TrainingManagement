import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Course, RequestDto, ResponseDto } from './course.models';

@Injectable({
  providedIn: 'root'
})
export class CourseApiService {
  private readonly courseUrl = `${environment.courseApiBaseUrl}/api/Course`;

  constructor(private readonly http: HttpClient) {}

  getCourses(): Observable<ResponseDto<Course[]>> {
    return this.http.get<ResponseDto<Course[]>>(this.courseUrl);
  }

  addCourse(course: Omit<Course, 'courseId'>): Observable<ResponseDto<unknown>> {
    return this.http.post<ResponseDto<unknown>>(this.courseUrl, this.createRequest('POST', [course]));
  }

  updateCourse(course: Course): Observable<ResponseDto<unknown>> {
    return this.http.put<ResponseDto<unknown>>(this.courseUrl, this.createRequest('PUT', course));
  }

  deleteCourse(courseId: number): Observable<ResponseDto<unknown>> {
    return this.http.delete<ResponseDto<unknown>>(`${this.courseUrl}/${courseId}`);
  }

  private createRequest<T>(method: string, data: T): RequestDto<T> {
    return {
      url: this.courseUrl,
      method,
      data
    };
  }
}
