import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';

import { CourseApiService } from './courses/course-api.service';
import { Course, CourseForm, ResponseDto } from './courses/course.models';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected courses: Course[] = [];
  protected courseForm: CourseForm = this.createEmptyForm();
  protected loading = false;
  protected saving = false;
  protected deletingCourseId: number | null = null;
  protected message = '';
  protected error = '';

  constructor(private readonly courseApi: CourseApiService) {}

  ngOnInit(): void {
    this.loadCourses();
  }

  protected get isEditing(): boolean {
    return this.courseForm.courseId !== null;
  }

  protected loadCourses(): void {
    this.loading = true;
    this.error = '';

    this.courseApi
      .getCourses()
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: (response) => {
          if (response.isRequestProcessed) {
            this.courses = response.data ?? [];
            return;
          }

          this.error = this.extractError(response, 'Courses could not be loaded.');
        },
        error: (err) => {
          this.error = err?.message ?? 'Courses could not be loaded.';
        }
      });
  }

  protected saveCourse(): void {
    this.error = '';
    this.message = '';

    const courseName = this.courseForm.courseName.trim();
    const courseDescription = this.courseForm.courseDescription.trim();

    if (!courseName || !courseDescription || this.courseForm.duration <= 0) {
      this.error = 'Enter a course name, description, and duration greater than zero.';
      return;
    }

    this.saving = true;

    const request = this.isEditing
      ? this.courseApi.updateCourse({
          courseId: this.courseForm.courseId ?? 0,
          courseName,
          courseDescription,
          duration: this.courseForm.duration
        })
      : this.courseApi.addCourse({
          courseName,
          courseDescription,
          duration: this.courseForm.duration
        });

    request.pipe(finalize(() => (this.saving = false))).subscribe({
      next: (response) => this.handleMutationResponse(response),
      error: (err) => {
        this.error = err?.message ?? 'Course could not be saved.';
      }
    });
  }

  protected editCourse(course: Course): void {
    this.error = '';
    this.message = '';
    this.courseForm = { ...course };
  }

  protected cancelEdit(): void {
    this.error = '';
    this.courseForm = this.createEmptyForm();
  }

  protected deleteCourse(course: Course): void {
    const confirmed = window.confirm(`Delete ${course.courseName}?`);
    if (!confirmed) {
      return;
    }

    this.error = '';
    this.message = '';
    this.deletingCourseId = course.courseId;

    this.courseApi
      .deleteCourse(course.courseId)
      .pipe(finalize(() => (this.deletingCourseId = null)))
      .subscribe({
        next: (response) => this.handleMutationResponse(response, 'Course deleted.'),
        error: (err) => {
          this.error = err?.message ?? 'Course could not be deleted.';
        }
      });
  }

  protected trackByCourseId(_index: number, course: Course): number {
    return course.courseId;
  }

  private handleMutationResponse(response: ResponseDto<unknown>, fallbackMessage = 'Course saved.'): void {
    if (!response.isRequestProcessed && response.statusCode !== 204) {
      this.error = this.extractError(response, 'The course change could not be completed.');
      return;
    }

    this.message = response.message || fallbackMessage;
    this.courseForm = this.createEmptyForm();
    this.loadCourses();
  }

  private extractError(response: ResponseDto<unknown>, fallback: string): string {
    return response.errors?.filter(Boolean).join(' ') || response.message || fallback;
  }

  private createEmptyForm(): CourseForm {
    return {
      courseId: null,
      courseName: '',
      courseDescription: '',
      duration: 1
    };
  }
}
