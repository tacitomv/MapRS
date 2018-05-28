// Sass configuration
var gulp = require('gulp'),
    sass = require('gulp-sass'),
    plumber = require('gulp-plumber');

gulp.task('sass', function() {
    gulp.src('./scss/styles.scss')
        .pipe(plumber())
        .pipe(sass())
        .pipe(gulp.dest('./wwwroot/css'))
});

gulp.task('default', ['sass'], function() {
    gulp.watch('./scss/**/*.scss', ['sass']);
})