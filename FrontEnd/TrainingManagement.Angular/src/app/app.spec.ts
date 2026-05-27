import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { App } from './app';
import { CourseApiService } from './courses/course-api.service';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        {
          provide: CourseApiService,
          useValue: {
            getCourses: () =>
              of({
                data: [],
                isRequestProcessed: true,
                message: 'Courses retrieved successfully',
                statusCode: 200,
                errors: null
              })
          }
        }
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should render heading', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Training Management');
  });
});
