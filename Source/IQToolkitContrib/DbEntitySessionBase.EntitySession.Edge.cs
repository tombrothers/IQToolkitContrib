using System;

namespace IQToolkitContrib {
    public abstract partial class DbEntitySessionBase {
        partial class EntitySession {
            // copied directly from IQToolkit.Data.EntitySession
            class Edge : IEquatable<Edge> {
                internal TrackedItem Source { get; private set; }
                internal TrackedItem Target { get; private set; }
                int hash;

                internal Edge(TrackedItem source, TrackedItem target) {
                    this.Source = source;
                    this.Target = target;
                    this.hash = this.Source.GetHashCode() + this.Target.GetHashCode();
                }

                public bool Equals(Edge edge) {
                    return edge != null && this.Source == edge.Source && this.Target == edge.Target;
                }

                public override bool Equals(object obj) {
                    return this.Equals(obj as Edge);
                }

                public override int GetHashCode() {
                    return this.hash;
                }
            }
        }
    }
}
