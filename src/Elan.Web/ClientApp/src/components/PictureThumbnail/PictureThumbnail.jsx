export default class PictureThumbnail extends Component {
    render() {
        return (
            <div className="col-md-3 image-box">
                <a href={this.props.targetUrl} className="thumbnail">
                    <img src={this.props.pictureSource} data-toggle="tooltip" data-placement="bottom" title={this.props.title} alt="" />
                </a>
            </div>
        );
    }
}