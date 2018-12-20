import React, { Component } from 'react';
import './PictureThumbnail.css';

export default class PictureThumbnail extends Component {
    constructor(props) {
        super(props);

        this.state = {
            showBigPictureModal: false
        }
    }

    bigPictureModalToggle = () => {
        this.setState({showBigPictureModal: !this.state.showBigPictureModal})
    }

    renderThumbnailLink = () => {
        if (this.props.targetUrl.trim() !== '') {
            return (
                <a href={this.props.targetUrl} className="thumbnail">
                    <img src={this.props.pictureSource} data-toggle="tooltip" data-placement="bottom" title={this.props.title} alt="" width="100%" />
                </a>
            )
        } else {
            return (
                <a onClick={() => this.bigPictureModalToggle()} className="thumbnail link">
                    <img src={this.props.pictureSource} data-toggle="tooltip" data-placement="bottom" title={this.props.title} alt="" width="100%" />
                </a>
            )
        }
    }

    render() {
        return (
            <div className="col-md-3 image-box">
                {this.renderThumbnailLink()}
                {this.renderBigPictureModal()}
            </div>
        );
    }

    renderBigPictureModal() {
        if (this.state.showBigPictureModal)
        return (
            <div className="modal show modal-fixed" tabIndex="-1" role="dialog">
                <div className="modal-dialog modal-lg" role="document">
                    <div className="modal-content">
                        <div className="modal-body">
                            <form>
                                <img className="thumbnail preview-image" src={this.props.pictureSource} />
                            </form>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" onClick={() => this.bigPictureModalToggle()}>Zamknij</button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}