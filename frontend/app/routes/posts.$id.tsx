import { BlogPostView } from "../components/BlogPostView";

export function meta() {
  return [
    { title: "Blog Post" },
  ];
}

export default function PostView() {
  return <BlogPostView />;
}