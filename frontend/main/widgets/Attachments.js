import 'isomorphic-fetch';
import React, { Component } from 'react';
import { Button, Typography, Grid, Icon, IconButton, Link } from '@material-ui/core';
import Dropzone from 'react-dropzone';
import AppConfig from '../core/AppConfig';
import AuthService from '../core/AuthService';

const Request = async (method, endpoint, data, BaseURL) => {
  if (AuthService.auth == null) AuthService.fillAuthData();
  if (!AuthService.auth || !AuthService.auth.user) throw 'User not signed in.';

  const config = {
    method: method,
    mode: 'cors',
    headers: {
      Authorization: `Bearer ${AuthService.auth.user.BearerToken}`
    },
    body: data
  };

  let response = await fetch((BaseURL || AppConfig.BaseURL) + endpoint, config);
  if (response) {
    if (!response.ok) throw await response.json();
    if (response.status == 403) alert('Invalid Role.');
    if (response.status == 401) throw response;
  } else {
    alert('Failed to fetch. Probably server is down.');
  }

  let json = await response.json();
  return json;
};

class Attachments extends Component {
  state = {
    files: [],
    targetFolder: '',
    uploading: false
  };

  constructor(props) {
    super(props);
    this.el = React.createRef();
  }

  updateFiles = from => {
    let files = [...from];
    this.setState({ files });
  };

  componentDidMount() {
    let { owner = {}, listBind = 'Attachments' } = this.props;
    if (owner[listBind]) {
      this.updateFiles(owner[listBind]);
    }
  }

  componentDidUpdate(prevProps, prevState) {
    const { uploading } = this.state;
    if (uploading) return;

    const { files: prevFiles, targetFolder: prevTargetFolder } = prevState;

    let { owner = {}, listBind = 'Attachments', folderBind = 'AttachmentsFolder' } = this.props;

    let targetFolder = owner[folderBind];
    if (targetFolder != prevTargetFolder) {
      this.setState({ targetFolder });
    }

    let files = owner[listBind];
    if (files && files.length != prevFiles.length) {
      this.updateFiles(files);
      return;
    }

    //TODO: Verify comparision:
    // if (files !== prevFiles) {
    //   console.log('files !== prevFiles, updating...');
    //   this.updateFiles(files);
    // }
  }

  onFilesAdded = addedFiles => {
    let { kind = '', onChange, listBind = 'Attachments' } = this.props;
    let { files, targetFolder } = this.state;

    let adaptedAddedFiles = addedFiles.map(file => {
      file.FileName = file.name;
      file.Directory = targetFolder;
      file.AttachmentKind = kind;
      file.isForUpload = true;
      return file;
    });

    files = [...files, ...adaptedAddedFiles];
    this.updateFiles(files);

    if (onChange) onChange(files, listBind);
  };

  uploadFiles = async () => {
    const { owner, listBind = 'Attachments', onChange } = this.props;
    let { files } = this.state;

    this.setState({ uploading: true });

    let filesToUpload = files.filter(file => file.isForUpload);
    try {
      for (let [index] of filesToUpload.entries()) {
        await this.sendRequest(index, filesToUpload);
      }

      files = [...this.state.files];
      let { targetFolder } = this.state;

      if (onChange) return onChange(files, listBind, targetFolder);

      return owner;
    } catch (e) {
      console.log(e);
      alert(JSON.stringify(e, null, 3));
    } finally {
      this.setState({ uploading: false });
    }
  };

  sendRequest = (index, arrToUpload) => {
    let file = arrToUpload[index];
    let { files, targetFolder } = this.state;

    const formData = new FormData();
    formData.append('file', file, file.FileName);
    formData.append('AttachmentKind', this.Kind);
    formData.append('TargetFolder', targetFolder);

    return Request('POST', 'Attachment.json', formData)
      .then(response => {
        let updatedFile;
        let updatedFiles = files.map(f => {
          if (f.FileName == file.FileName) {
            updatedFile = { ...f };
            updatedFile.isForUpload = false;
            return updatedFile;
          }
          return f;
        });
        if (targetFolder != response.Directory) {
          this.setState({ targetFolder: response.Directory });
        }
        this.updateFiles(updatedFiles);
        return updatedFile;
      })
      .catch(() => (file.status = 'error'));
  };

  openDialog = () => this.el.current.open && this.el.current.open();

  removeFile = (file, index) => {
    const { listBind = 'Attachments', onChange } = this.props;
    let { files } = this.state;
    let updatedFiles;
    if (file.isForUpload) updatedFiles = files.filter((f, i) => i != index);
    else
      updatedFiles = files.map((f, i) => {
        if (i == index) {
          let updatedFile = { ...f };
          updatedFile.ToDelete = true;
          return updatedFile;
        }
        return f;
      });

    this.updateFiles(updatedFiles);
    if (onChange) onChange(updatedFiles, listBind);
  };

  cancelRemove = (file, index) => {
    const { listBind = 'Attachments', onChange } = this.props;
    let { files } = this.state;
    let updatedFiles = files.map((f, i) => {
      if (i == index) {
        let updatedFile = { ...f };
        updatedFile.ToDelete = false;
        return updatedFile;
      }
      return f;
    });
    this.updateFiles(updatedFiles);
    if (onChange) onChange(updatedFiles, listBind);
  };

  render() {
    console.log('Render Attachments');

    let { files } = this.state;
    let { owner = {}, kind = '', onChange, afterDelete, listBind, folderBind = 'AttachmentsFolder', printMode, readOnly } = this.props;

    const api = listBind ? 'api_' + listBind : 'api_attachments';
    owner[api] = {};
    owner[api].uploadFiles = this.uploadFiles;

    this.Kind = kind;

    return (
      <>
        {/* <pre>{JSON.stringify(this.state.targetFolder, null, 3)}</pre> */}
        <Dropzone ref={this.el} multiple onDrop={this.onFilesAdded} noClick>
          {({ getRootProps, getInputProps }) => (
            <Grid container direction='column' {...getRootProps()} className='Attachments well' onDoubleClick={this.openDialog}>
              <input {...getInputProps()} style={{ display: 'none' }} />
              {files.map((file, index) => {
                return (
                  <Grid container direction='row' key={file.FileName} alignItems='flex-end' className='AttachmentsRow'>
                    {file.isForUpload && <Icon style={{ margin: '0 2px' }}>cloud_upload</Icon>}
                    <Grid item xs>
                      <a
                        target='_blank'
                        href={
                          AppConfig.BaseURL +
                          `Attachment/Download?directory=${file.Directory}&filename=${file.FileName}&attachmentKind=${kind}`
                        }
                        style={{
                          cursor: 'pointer',
                          textDecoration: file.ToDelete ? 'line-through' : 'initial',
                          color: file.ToDelete ? 'red' : '',
                          width: '100%',
                          display: 'inline-block'
                        }}
                      >
                        <Typography variant='caption' className={file.status}>
                          {file.FileName}
                        </Typography>
                      </a>
                    </Grid>
                    {!printMode && !readOnly && !file.ToDelete && (
                      <IconButton size='small' color='secondary' onClick={() => this.removeFile(file, index)}>
                        <Icon style={{ fontSize: '1em' }}>close</Icon>
                      </IconButton>
                    )}
                    {!printMode && !readOnly && file.ToDelete && (
                      <Button
                        size='small'
                        variant='text'
                        style={{ fontSize: '.6em' }}
                        color='secondary'
                        onClick={() => this.cancelRemove(file, index)}
                      >
                        (cancel remove)
                      </Button>
                    )}
                  </Grid>
                );
              })}
              <Grid container direction='row' alignItems='center'>
                {!readOnly && (
                  <Grid item xs>
                    <Button
                      className='hidden-print'
                      variant='contained'
                      color='primary'
                      size='small'
                      onClick={this.openDialog}
                      style={{ margin: 3, fontSize: '.65em', padding: 1 }}
                    >
                      Add Files
                    </Button>
                  </Grid>
                )}
                {!files.length && (
                  <Grid item xs>
                    <Typography variant='caption'>No Files</Typography>
                  </Grid>
                )}
              </Grid>
            </Grid>
          )}
        </Dropzone>
      </>
    );
  }
}

export default Attachments;
