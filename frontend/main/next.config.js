const withSass = require('@zeit/next-sass');
const withFonts = require('next-fonts');
const withBundleAnalyzer = require('@zeit/next-bundle-analyzer');
// const isProd = process.env.NODE_ENV === 'production';

module.exports = withBundleAnalyzer(
  withFonts(
    withSass({
      target: 'serverless',
      // assetPrefix: isProd ? 'http://jucvwdoccnl01/UniversalCatalogs/main' : '',
      // exportPathMap: function() {
      //   return {
      //     '/': { page: '/' },
      //     '/catalog-type': { page: '/catalog-type' },
      //     '/catalog': { page: '/catalog' },
      //     '/catalogs': { page: '/catalogs' }
      //   };
      // },
      analyzeServer: ['server', 'both'].includes(process.env.BUNDLE_ANALYZE),
      analyzeBrowser: ['browser', 'both'].includes(process.env.BUNDLE_ANALYZE),
      bundleAnalyzerConfig: {
        server: {
          analyzerMode: 'static',
          reportFilename: '../bundles/server.html'
        },
        browser: {
          analyserMode: 'static',
          reportFilename: '../bundles/client.html'
        }
      }
    })
  )
);
