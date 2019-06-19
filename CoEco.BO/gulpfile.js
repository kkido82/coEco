//var gulp = require('gulp'),
//    concat = require("gulp-concat"),
//    sourcemaps = require('gulp-sourcemaps'),
//    cssmin = require("gulp-cssmin"),
//    babel = require('gulp-babel'),
//    htmlmin = require("gulp-htmlmin"),
//    uglify = require("gulp-uglify"),
//    merge = require("merge-stream"),
//    jshint = require("gulp-jshint"),
//    gutil = require('gulp-util'),
//    plumber = require('gulp-plumber'),
//    del = require("del"),
//    sequence = require('run-sequence'),
//    clean = require('gulp-clean'),
//    bundleconfig = [{ "outputFileName": "app.min.js", "inputFiles": ["app/**/*.js", "!app/**/*.min.js"] }];


//gulp.task('clean', () => {
//    return gulp.src('temp/*', { read: false })
//        .pipe(clean());
//});

//gulp.task('babel', () => {
//    return babelPipeline(["app/**/*.js", "!app/**/*.min.js"]);
//});

//gulp.task('bundle', () => {
//    return gulp.src(["temp/**/*.js", "!temp/**/*.min.js"])
//        .pipe(sourcemaps.init({ loadMaps: true }))
//        .pipe(concat('app.min.js'))
//        .pipe(sourcemaps.write())
//        .pipe(gulp.dest("./app/"));
//});


//gulp.task('build', () => {
//    return sequence('babel', 'bundle');
//});


//gulp.task('watch-bundle', () => {
//    return gulp.watch(["temp/**/*.js", "!app/**/*.min.js"], ['bundle']);
//})

//gulp.task('watch', () => {
//    sequence('babel', 'bundle', 'watch-bundle');
//    return gulp.watch(['app/**/*.js', '!app/**/*.min.js'], change => {
//        if (change.type === 'changed') {
//            return babelPipeline(change.path);
//        }
//    });
//});

//function babelPipeline(src) {
//    return gulp.src(src)
//        .pipe(sourcemaps.init())
//        .pipe(babel({ compact: true, presets: ['es2015'] }))
//        .pipe(sourcemaps.write())
//        .pipe(gulp.dest("./temp/"))
//        .pipe(concat('app.min.js'));
//}



///// <binding ProjectOpened='watch, min:js' />
///*
//This file in the main entry point for defining Gulp tasks and using Gulp plugins.
//Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
//*/

var gulp = require('gulp'),
concat = require("gulp-concat"),
sourcemaps = require('gulp-sourcemaps');
cssmin = require("gulp-cssmin"),
babel = require('gulp-babel'),
htmlmin = require("gulp-htmlmin"),
uglify = require("gulp-uglify"),
merge = require("merge-stream"),
jshint = require("gulp-jshint"),
gutil = require('gulp-util'),
plumber = require('gulp-plumber'),
del = require("del"),
bundleconfig = [{ "outputFileName": "app.min.js", "inputFiles": ["app/**/*.js", "!app/**/*.min.js"] }];

gulp.task("min:js", function () {
    var tasks = getBundles(".js").map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(plumber())
            .pipe(sourcemaps.init())
            .pipe(jshint({ esversion: 6 }).on('error', gutil.log))
            .pipe(jshint.reporter('default', { fail: true }).on('error', gutil.log))
            .pipe(babel({ presets: ['es2015'] }).on('error', gutil.log))
            .pipe(concat(bundle.outputFileName).on('error', gutil.log))
            //.pipe(uglify().on('error', gutil.log))
             .pipe(sourcemaps.write('../app/').on('error', gutil.log))
            .pipe(gulp.dest("./app/"));
    });
    return merge(tasks);
});

gulp.task("productionBuild", function () {
    var tasks = getBundles(".js").map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(plumber())
            .pipe(sourcemaps.init())
            .pipe(jshint({ esversion: 6 }).on('error', gutil.log))
            .pipe(jshint.reporter('default', { fail: true }).on('error', gutil.log))
             .pipe(babel({
                 presets: ['es2015']
             }).on('error', gutil.log))
            .pipe(concat(bundle.outputFileName).on('error', gutil.log))
            .pipe(uglify().on('error', gutil.log))
             .pipe(sourcemaps.write('../app/').on('error', gutil.log))
            .pipe(gulp.dest("./app/"));
    });
    return merge(tasks);
});

gulp.task('watch', function () {
    gulp.watch(bundleconfig[0].inputFiles, ['min:js']).on('error',
        function (err) {
            console.log(err.toString());
            this.emit('end');
        });
});

gulp.task('default', ['min:js', 'watch']);

function getBundles(extension) {
    return bundleconfig.filter(function (bundle) {
        return new RegExp(`${extension}$`).test(bundle.outputFileName);
    });
}