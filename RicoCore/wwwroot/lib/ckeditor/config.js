/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here.
	// For complete reference see:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config

	// The toolbar groups arrangement, optimized for two toolbar rows.
	config.toolbarGroups = [
		{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
		{ name: 'editing',     groups: [ 'find', 'selection', 'spellchecker' ] },
		{ name: 'links' },
		{ name: 'insert' },
		{ name: 'forms' },
		{ name: 'tools' },
		{ name: 'document',	   groups: [ 'mode', 'document', 'doctools' ] },
		{ name: 'others' },
		'/',
		{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
		{ name: 'paragraph',   groups: [ 'list', 'indent', 'blocks', 'align', 'bidi' ] },
		{ name: 'styles' },
		{ name: 'colors' },
		{ name: 'about' },
		{ name: 'codeSnippet' },
	];

	// Remove some buttons provided by the standard plugins, which are
	// not needed in the Standard(s) toolbar.
	config.removeButtons = 'Underline,Subscript,Superscript';
	config.extraPlugins = 'colorbutton';
	
	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;pre';
	
	// Simplify the dialog windows.
    config.removeDialogTabs = 'image:advanced;link:advanced';
    config.filebrowserUploadMethod = 'form';
	config.filebrowserImageUploadUrl = "/Admin/Upload/UploadImageForCKEditor",
	
	// Create a new plugin which registers a custom code highlighter
	// based on customEngine in order to replace the one that comes
	// with the Code Snippet plugin.
	CKEDITOR.plugins.add('myCustomHighlighter', {
		afterInit: function (editor) {
			// Create a new instance of the highlighter.
			var myHighlighter = new CKEDITOR.plugins.codesnippet.highlighter({
				init: function (ready) {
					// Asynchronous code to load resources and libraries for customEngine.
					customEngine.loadResources(function () {
						// Let the editor know that everything is ready.
						ready();
					});
				},
				highlighter: function (code, language, callback) {
					// Let the customEngine highlight the code.
					customEngine.highlight(code, language, function () {
						callback(highlightedCode);
					});
				}
			});

			// Check how it performs.
			myHighlighter.highlight('foo()', 'javascript', function (highlightedCode) {
				console.log(highlightedCode); // -> <span class="pretty">foo()</span>
			});

			// From now on, myHighlighter will be used as a Code Snippet
			// highlighter, overwriting the default engine.
			editor.plugins.codesnippet.setHighlighter(myHighlighter);
		}
	});

	config.extraPlugins = 'codeSnippet';
	config.codeSnippet_theme = 'default';
	config.codeSnippet_languages = {
		javascript: 'JavaScript',
	};

};
