/**
 * Initilaize RichTextEditor from React element
 */
import { HtmlEditor, Image, Inject, Link, QuickToolbar, RichTextEditorComponent, Toolbar } from '@syncfusion/ej2-react-richtexteditor';
import * as React from 'react';
import AppConfig from '../core/AppConfig';

class App extends React.Component {
  constructor(props) {
    super(props);
    this.el = React.createRef();
  }
  handleToolbarClick = event => {
    // let el = this.el.current;
    // el.imageModule.uploadObj.uploading = function(args) {
    //   args.customFormData.push('AttachmentKind', 'NCN');
    // };
  };
  render() {
    const { attachmentKind = 'Public', targetFolder = 'Public' } = this.props;
    let saveQueryParams = `AttachmentKind=${attachmentKind}&TargetFolder=${targetFolder}`;

    return (
      <RichTextEditorComponent
        ref={this.el}
        {...this.props}
        insertImageSettings={{
          saveUrl: AppConfig.BaseURL + 'Attachment.json?' + saveQueryParams,
          path: AppConfig.BaseURL + 'Attachment/download/'
        }}
        toolbarClick={this.handleToolbarClick}
      >
        <Inject services={[Toolbar, Image, Link, HtmlEditor, QuickToolbar]} />
      </RichTextEditorComponent>
    );
  }
}

export default App;
