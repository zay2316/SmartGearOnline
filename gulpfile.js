const gulp = require('gulp');
const sass = require('gulp-sass')(require('sass'));
const path = require('path');

gulp.task('sass', function () {
    return gulp.src('SmartGearOnline/wwwroot/scss/**/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('SmartGearOnline/wwwroot/css'));
});

gulp.task('watch', function () {
    gulp.watch('SmartGearOnline/wwwroot/scss/**/*.scss', gulp.series('sass'));
});