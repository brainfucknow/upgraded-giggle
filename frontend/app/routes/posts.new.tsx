import { BlogPostForm } from "../components/BlogPostForm";

export function meta() {
  return [
    { title: "Create New Blog Post" },
  ];
}

export default function PostNew() {
  return <BlogPostForm />;
}